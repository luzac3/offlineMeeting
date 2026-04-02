declare global {
    interface ElectronPrintArgs {
        resultIds: string[];
    }

    interface ElectronAPI {
        printReceipt: (args: ElectronPrintArgs) => Promise<{ ok: boolean; reason?: string }>;
        printCheckinQr: (args: { userCd: string }) => Promise<void>;
    }

    // iOS WKWebView メッセージハンドラ
    interface WKMessageHandler {
        postMessage: (body: any) => void;
    }

    interface Window {
        electronAPI?: ElectronAPI;
        webkit?: {
            messageHandlers?: {
                printReceipt?: WKMessageHandler;
                printCheckinQr?: WKMessageHandler;
            };
        };
    }
}

export { };