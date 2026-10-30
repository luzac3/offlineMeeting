import { CreateLog } from '../share/CreateLog';
import { BrowserWindow } from 'electron';
import { GetSetting } from '../share/GetSetting';

export class Printer {
    printWindow: BrowserWindow;
    url: string;
    data: any;
    constructor(id: string, urlBase?: string) {
        this.data = GetSetting.Setting();
        this.url = (urlBase ?? this.data.printUrlBase) + id;;

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

            // Linux (lp コマンド) 経由で印刷
            if (this.data.isLinuxLpPrint) {
                this.PrintWithLinuxLp(printOptions, (success, failureReason) => {
                    if (!success) {
                        const msg = `Printer.Print: linux lp print failed url=${this.url} reason=${failureReason}`;
                        CreateLog.WriteLog(msg);
                    }
                    try { this.printWindow.close(); } catch (e) { /* ignore */ }
                });
                return;
            }

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

    private PrintWithLinuxLp = async (printOptions: any, callback: (success: boolean, failureReason?: string) => void) => {
        const fs = require('fs');
        const { exec } = require('child_process');

        let tempPath: string | undefined;
        try {
            // 画面をPDF化してlpコマンドへ渡す
            const pdfData = await this.printWindow.webContents.printToPDF({
                marginsType: 1,
                printBackground: true,
                ...printOptions
            });

            tempPath = `/tmp/print_${Date.now()}.pdf`;
            await fs.promises.writeFile(tempPath, pdfData);

            const printerName = this.data.deviceName || 'Generic_ESC_POS';
            exec(`lp -d ${printerName} ${tempPath}`, (error: Error | null, stdout: string, stderr: string) => {
                if (error) {
                    callback(false, stderr || error.message);
                } else {
                    callback(true);
                }
                if (tempPath) {
                    fs.promises.unlink(tempPath).catch(() => { /* ignore */ });
                }
            });
        } catch (err: unknown) {
            const errMsg = err instanceof Error ? (err.stack ?? err.message) : String(err);
            callback(false, errMsg);
            if (tempPath) {
                fs.promises.unlink(tempPath).catch(() => { /* ignore */ });
            }
        }
    }
}