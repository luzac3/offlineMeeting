declare global {
    interface ElectronPrintArgs {
        url: string[];
    }

    interface ElectronAPI {
        printReceipt: (args: ElectronPrintArgs) => Promise<{ ok: boolean; reason?: string }>;
    }

    interface Window {
        electronAPI?: ElectronAPI;
    }
}

export { };