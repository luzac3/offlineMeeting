import { CreateLog } from '../share/CreateLog'
import { BrowserWindow } from 'electron'
import path from 'path';

export class CreateWindow {
    CreateWindow = (url: string, show = true) => {
        const win = new BrowserWindow({
            width: 800,
            height: 600,
            show: false,
            webPreferences: {
                preload: path.join(__dirname, "preload.js"),
                contextIsolation: true,
                nodeIntegration: false
            }
        });

        // 読み込み完了で表示
        win.once('ready-to-show', () => {
            if (show) win.show();
        });

        try {
            win.loadURL(url);
        } catch (err) {
            const errMsg = err instanceof Error ? (err.stack ?? err.message) : String(err);
            const msg = `loadURL failed url=${url} err=${errMsg}`;
            CreateLog.WriteLog(msg);
        }

        // did-fail-load を監視してログ出力
        win.webContents.on('did-fail-load', (event, errorCode, errorDescription, validatedURL, isMainFrame) => {
            const msg = `did-fail-load url=${validatedURL} code=${errorCode} desc=${errorDescription} isMainFrame=${isMainFrame}`;
            CreateLog.WriteLog(msg);
        });

        return win;
    }
}