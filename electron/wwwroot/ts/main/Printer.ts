import { CreateLog } from '../share/CreateLog';
import { BrowserWindow } from 'electron';

export class Printer {
    static Print = async (event: any, args: { url: string }) => {
        try {
            const url = typeof args === 'string' ? args : args?.url;
            if (!url) {
                const msg = 'Printer.Print: no url provided';
                console.error(msg);
                void CreateLog.WriteLog(msg);
                return;
            }

            const printWindow = new BrowserWindow({
                show: false,
                webPreferences: {
                    contextIsolation: true
                }
            });
            try {
                await printWindow.loadURL(url);
            } catch (loadErr: unknown) {
                const errMsg = loadErr instanceof Error ? (loadErr.stack ?? loadErr.message) : String(loadErr);
                const msg = `Printer.Print: loadURL failed url=${url} err=${errMsg}`;
                CreateLog.WriteLog(msg);
            }

            printWindow.webContents.on('did-finish-load', () => {
                try {
                    printWindow.webContents.print({
                        silent: true,
                        deviceName: 'Brother HL-2240D series'
                    }, (success, failureReason) => {
                        if (!success) {
                            const msg = `Printer.Print: print failed url=${url} reason=${failureReason}`;
                            CreateLog.WriteLog(msg);
                        }
                        try { printWindow.close(); } catch (e) { /* ignore */ }
                    });
                } catch (printErr: unknown) {
                    const errMsg = printErr instanceof Error ? (printErr.stack ?? printErr.message) : String(printErr);
                    const msg = `Printer.Print: print exception url=${url}} err=${errMsg}`;
                    CreateLog.WriteLog(msg);
                    try { printWindow.close(); } catch (e) { /* ignore */ }
                }
            });
        } catch (err) {
            const msg = `Printer.Print: unexpected error ${err && (err as any).stack ? (err as any).stack : String(err)}`;
            CreateLog.WriteLog(msg);
        }
    }
}