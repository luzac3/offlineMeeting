declare global {
    interface ElectronPrintArgs {
        template: string;
        apiPath: string;
        headers?: Record<string, string>;
    }

    interface ElectronAPI {
        printWithTemplate: (args: ElectronPrintArgs) => Promise<{ ok: boolean; reason?: string }>;
    }

    interface Window {
        electronAPI?: ElectronAPI;
    }
}

export { };