import UIKit
import WebKit

/// Electron の Printer.ts に相当
/// 非表示WKWebViewでHTMLを読み込み → UIPrintInteractionController でサイレント印刷
///
/// - 初回: プリンタ選択ダイアログ表示 → 選択結果を保存
/// - 2回目以降: 保存済みプリンタへ自動印刷（ダイアログなし）
class PrintService: ObservableObject {
    /// 保存済みプリンタ（UserDefaults に URL を永続化）
    @Published var savedPrinter: UIPrinter?

    private let printerURLKey = "SavedPrinterURL"
    private let printerNameKey = "SavedPrinterName"

    /// 保存済みプリンタの表示名
    var savedPrinterName: String {
        if let name = UserDefaults.standard.string(forKey: printerNameKey), !name.isEmpty {
            return name
        }
        return savedPrinter != nil ? "(名前未取得)" : "未設定"
    }

    init() {
        restoreSavedPrinter()
    }

    // MARK: - Public

    /// URL を読み込んで印刷（Electron の Printer.Print() 相当）
    func printFromURL(_ url: URL) {
        let config = WKWebViewConfiguration()
        let webView = WKWebView(frame: CGRect(x: 0, y: 0, width: 580, height: 900), configuration: config)
        // 画面外に配置（非表示、Electronの show:false 相当）
        webView.isHidden = true

        // UIWindowに追加しないとレンダリングされない
        if let windowScene = UIApplication.shared.connectedScenes.first as? UIWindowScene,
           let window = windowScene.windows.first {
            window.addSubview(webView)
        }

        let request = URLRequest(url: url, cachePolicy: .reloadIgnoringLocalCacheData)
        webView.load(request)

        // 読み込み完了を監視して印刷を実行
        let observer = WebViewLoadObserver(webView: webView) { [weak self] in
            self?.printWebView(webView)
        }
        // observer を webView に紐付けて保持
        objc_setAssociatedObject(webView, "observer", observer, .OBJC_ASSOCIATION_RETAIN)
    }

    /// プリンタ選択ダイアログを表示
    func showPrinterPicker() {
        let picker = UIPrinterPickerController(initiallySelectedPrinter: savedPrinter)

        guard let windowScene = UIApplication.shared.connectedScenes.first as? UIWindowScene,
              let rootVC = windowScene.windows.first?.rootViewController else { return }

        picker.present(animated: true) { [weak self] controller, userDidSelect, error in
            if userDidSelect, let printer = controller.selectedPrinter {
                self?.savePrinter(printer)
            }
        }
    }

    // MARK: - Private

    private func printWebView(_ webView: WKWebView) {
        let printInfo = UIPrintInfo.printInfo()
        printInfo.outputType = .general
        printInfo.jobName = "OfflineMeeting"

        let formatter = webView.viewPrintFormatter()

        if let printer = savedPrinter {
            // 保存済みプリンタがある → ダイアログなしで直接印刷
            // （Electron の silent: true, deviceName: "POS-80C" 相当）
            let controller = UIPrintInteractionController.shared
            controller.printInfo = printInfo
            controller.printFormatter = formatter

            controller.print(to: printer) { _, completed, error in
                if let error = error {
                    print("Print error: \(error.localizedDescription)")
                }
                self.cleanUpWebView(webView)
            }
        } else {
            // プリンタ未設定 → ダイアログ表示（初回のみ）
            let controller = UIPrintInteractionController.shared
            controller.printInfo = printInfo
            controller.printFormatter = formatter

            guard let windowScene = UIApplication.shared.connectedScenes.first as? UIWindowScene,
                  let rootVC = windowScene.windows.first?.rootViewController else {
                cleanUpWebView(webView)
                return
            }

            controller.present(animated: true) { [weak self] _, completed, error in
                if let error = error {
                    print("Print error: \(error.localizedDescription)")
                }
                self?.cleanUpWebView(webView)
            }
        }
    }

    private func cleanUpWebView(_ webView: WKWebView) {
        webView.removeFromSuperview()
        objc_setAssociatedObject(webView, "observer", nil, .OBJC_ASSOCIATION_RETAIN)
    }

    // MARK: - プリンタ永続化

    private func savePrinter(_ printer: UIPrinter) {
        savedPrinter = printer
        UserDefaults.standard.set(printer.url.absoluteString, forKey: printerURLKey)
        // displayName はコンタクト後に取得可能。非同期で取得して保存
        printer.contactPrinter { available in
            if available {
                UserDefaults.standard.set(printer.displayName, forKey: self.printerNameKey)
                DispatchQueue.main.async { self.objectWillChange.send() }
            }
        }
    }

    /// プリンタ設定をクリア（設定画面から呼び出し）
    func clearPrinter() {
        savedPrinter = nil
        UserDefaults.standard.removeObject(forKey: printerURLKey)
        UserDefaults.standard.removeObject(forKey: printerNameKey)
    }

    private func restoreSavedPrinter() {
        guard let urlString = UserDefaults.standard.string(forKey: printerURLKey),
              let url = URL(string: urlString) else { return }
        savedPrinter = UIPrinter(url: url)
    }
}

// MARK: - WKNavigationDelegate でロード完了を検知

private class WebViewLoadObserver: NSObject, WKNavigationDelegate {
    private let onFinish: () -> Void
    private weak var webView: WKWebView?

    init(webView: WKWebView, onFinish: @escaping () -> Void) {
        self.onFinish = onFinish
        self.webView = webView
        super.init()
        webView.navigationDelegate = self
    }

    func webView(_ webView: WKWebView, didFinish navigation: WKNavigation!) {
        // 少し待ってからレンダリング完了を保証（QRコード生成等の非同期処理対応）
        DispatchQueue.main.asyncAfter(deadline: .now() + 0.5) { [weak self] in
            self?.onFinish()
        }
    }

    func webView(_ webView: WKWebView, didFail navigation: WKNavigation!, withError error: Error) {
        print("Print page load failed: \(error.localizedDescription)")
        webView.removeFromSuperview()
    }
}
