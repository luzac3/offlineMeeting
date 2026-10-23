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
            this.printWindow.webContents.once('did-finish-load', async () => {
                await this.WaitForRender();
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

    // QRコードなどの非同期描画が完了し、レイアウトが確定してから印刷するために待機する。
    // ページ内の全画像・フォントの読み込み完了と、描画フレームの反映を待つ。
    // 失敗してもログを残して処理を継続し、印刷自体は止めない。
    private WaitForRender = async (): Promise<void> => {
        try {
            await this.printWindow.webContents.executeJavaScript(`
                new Promise((resolve) => {
                    const done = () => {
                        // レイアウト確定後の描画フレームを2回待ってから解決する
                        requestAnimationFrame(() => requestAnimationFrame(resolve));
                    };
                    const waitForImages = () => {
                        const images = Array.from(document.images || []);
                        const pending = images
                            .filter((img) => !img.complete || img.naturalWidth === 0)
                            .map((img) => new Promise((res) => {
                                img.addEventListener('load', res, { once: true });
                                img.addEventListener('error', res, { once: true });
                            }));
                        const fonts = (document.fonts && document.fonts.ready)
                            ? document.fonts.ready
                            : Promise.resolve();
                        Promise.all([...pending, fonts]).then(done);
                    };
                    if (document.readyState === 'complete') {
                        waitForImages();
                    } else {
                        window.addEventListener('load', waitForImages, { once: true });
                    }
                });
            `);
        } catch (waitErr: unknown) {
            const errMsg = waitErr instanceof Error ? (waitErr.stack ?? waitErr.message) : String(waitErr);
            const msg = `Printer.WaitForRender: wait failed url=${this.url} err=${errMsg}`;
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
