import WebKit

/// WKScriptMessageHandler — Web側の window.webkit.messageHandlers.xxx 呼び出しをSwift側で受け取る
/// Web側は window.electronAPI (Electron) / window.webkit.messageHandlers (iOS) を併記して呼び分ける
class JSBridge: NSObject, WKScriptMessageHandler {
    private let settings: AppSettings
    private let printService: PrintService

    init(settings: AppSettings, printService: PrintService) {
        self.settings = settings
        self.printService = printService
    }

    /// WKWebViewConfiguration にメッセージハンドラを登録
    static func configure(
        _ config: WKWebViewConfiguration,
        settings: AppSettings,
        printService: PrintService
    ) -> JSBridge {
        let bridge = JSBridge(settings: settings, printService: printService)
        let controller = config.userContentController
        controller.add(bridge, name: "printReceipt")
        controller.add(bridge, name: "printCheckinQr")
        return bridge
    }

    // MARK: - WKScriptMessageHandler

    func userContentController(
        _ userContentController: WKUserContentController,
        didReceive message: WKScriptMessage
    ) {
        switch message.name {
        case "printReceipt":
            handlePrintReceipt(message.body)
        case "printCheckinQr":
            handlePrintCheckinQr(message.body)
        default:
            break
        }
    }

    // MARK: - 印刷ハンドラ

    /// printReceipt: 印刷用URLを読み込んでサイレント印刷
    private func handlePrintReceipt(_ body: Any) {
        guard let dict = body as? [String: Any],
              let resultIds = dict["resultIds"] as? [Any] else { return }

        let baseUrl = settings.printUrlBase
        for id in resultIds {
            let urlString = "\(baseUrl)\(id)"
            if let url = URL(string: urlString) {
                printService.printFromURL(url)
            }
        }
    }

    /// チェックインQR印刷
    private func handlePrintCheckinQr(_ body: Any) {
        guard let dict = body as? [String: Any],
              let userCd = dict["userCd"] else { return }

        let baseUrl = settings.checkinPrintUrlBase
        let urlString = "\(baseUrl)\(userCd)"
        if let url = URL(string: urlString) {
            printService.printFromURL(url)
        }
    }
}
