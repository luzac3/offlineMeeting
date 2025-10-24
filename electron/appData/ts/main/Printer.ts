import { CreateLog } from '../share/CreateLog';
import { BrowserWindow } from 'electron';
import fs from 'fs';

export class Printer {
    static Print = async (event: any, args: { resultIds: string[] }) => {
        try {
            const bufferData = fs.readFileSync('./appData/json/setting.json');
            const data = JSON.parse(bufferData.toString());

            const printWindow = new BrowserWindow({
                show: false,
                webPreferences: {
                    contextIsolation: true
                }
            });
            args.resultIds.forEach(async (resultId) => {
                const url = data.printUrlBase + resultId;
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
                            deviceName: data.deviceName
                        }, (success, failureReason) => {
                            if (!success) {
                                const msg = `Printer.Print: print failed url=${url} reason=${failureReason}`;
                                CreateLog.WriteLog(msg);
                            }
                            try { printWindow.close(); } catch (e) { /* ignore */ }
                        });
                    } catch (printErr: unknown) {
                        const errMsg = printErr instanceof Error ? (printErr.stack ?? printErr.message) : String(printErr);
                        const msg = `Printer.Print: print exception url=${url} err=${errMsg}`;
                        CreateLog.WriteLog(msg);
                        try { printWindow.close(); } catch (e) { /* ignore */ }
                    }
                });
            });
        } catch (err) {
            const msg = `Printer.Print: unexpected error ${err && (err as any).stack ? (err as any).stack : String(err)}`;
            CreateLog.WriteLog(msg);
        }
    }
}