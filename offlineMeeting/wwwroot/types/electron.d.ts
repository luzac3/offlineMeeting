declare global {
    interface ElectronPrintArgs {
        resultIds: string[];
    }

    interface ElectronAPI {
        printReceipt: (args: ElectronPrintArgs) => Promise<{ ok: boolean; reason?: string }>;
        printCheckinQr: (args: { userCd: string }) => Promise<void>;
    }

    interface Window {
        electronAPI?: ElectronAPI;
    }
}

export { };