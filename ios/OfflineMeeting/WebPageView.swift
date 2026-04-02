import SwiftUI
import WebKit

/// Electron の CreateWindow.CreateWindow() に相当
/// WKWebView + JSブリッジ付きのWebページ表示
struct WebPageView: View {
    let url: URL
    @EnvironmentObject private var settings: AppSettings
    @EnvironmentObject private var printService: PrintService
    @Environment(\.dismiss) private var dismiss

    var body: some View {
        NavigationStack {
            WebViewRepresentable(url: url, settings: settings, printService: printService)
                .toolbar {
                    ToolbarItem(placement: .navigationBarLeading) {
                        Button("メニューに戻る") { dismiss() }
                    }
                }
        }
    }
}

/// UIViewRepresentable で WKWebView をSwiftUIに統合
struct WebViewRepresentable: UIViewRepresentable {
    let url: URL
    let settings: AppSettings
    let printService: PrintService

    func makeCoordinator() -> Coordinator {
        Coordinator()
    }

    func makeUIView(context: Context) -> WKWebView {
        let config = WKWebViewConfiguration()
        config.allowsInlineMediaPlayback = true

        // JSブリッジ登録（window.electronAPI を注入）
        let bridge = JSBridge.configure(config, settings: settings, printService: printService)
        context.coordinator.bridge = bridge

        let webView = WKWebView(frame: .zero, configuration: config)
        webView.navigationDelegate = context.coordinator
        webView.allowsBackForwardNavigationGestures = true
        webView.load(URLRequest(url: url))
        return webView
    }

    func updateUIView(_ webView: WKWebView, context: Context) {}

    class Coordinator: NSObject, WKNavigationDelegate {
        /// JSBridge の参照保持（ARC解放防止）
        var bridge: JSBridge?
    }
}
