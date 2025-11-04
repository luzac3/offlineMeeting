import { CreateLog } from '../share/CreateLog';
import { BrowserWindow } from 'electron';
import { GetSetting } from '../share/GetSetting';

export class Printer {
    printWindow: BrowserWindow;
    url: string;
    data: any;
    constructor(resultId: string) {
        this.data = GetSetting.Setting();
        this.url = this.data.printUrlBase + resultId;;

        this.printWindow = new BrowserWindow({
            show: false,
            webPreferences: {
                contextIsolation: true
            }
        });

    }

    Print = async () => {
        try {
            this.printWindow.webContents.once('did-finish-load', () => {
                this.LoadedPrint();
            });

            try {
                await this.printWindow.loadURL(this.url);
            } catch (loadErr: unknown) {
                const errMsg = loadErr instanceof Error ? (loadErr.stack ?? loadErr.message) : String(loadErr);
                const msg = `Printer.Print: loadURL failed url=${this.url} err=${errMsg}`;
                CreateLog.WriteLog(msg);
            }
        } catch (err) {
            const msg = `Printer.Print: unexpected error ${err && (err as any).stack ? (err as any).stack : String(err)}`;
            CreateLog.WriteLog(msg);
        }
    }

    private LoadedPrint = () => {
        try {
            const printOptions = this.data.printOptions || {};

            this.printWindow.webContents.print({
                silent: true,
                deviceName: this.data.deviceName,
                ...printOptions
            }, (success, failureReason) => {
                if (!success) {
                    const msg = `Printer.Print: print failed url=${this.url} reason=${failureReason}`;
                    CreateLog.WriteLog(msg);
                }
                try { this.printWindow.close(); } catch (e) { /* ignore */ }
            });
        } catch (printErr: unknown) {
            const errMsg = printErr instanceof Error ? (printErr.stack ?? printErr.message) : String(printErr);
            const msg = `Printer.Print: print exception url=${this.url} err=${errMsg}`;
            CreateLog.WriteLog(msg);
            try { this.printWindow.close(); } catch (e) { /* ignore */ }
        }
    }
}