import { CreateLog } from '../share/CreateLog'
import { app, BrowserWindow } from 'electron'
import path from 'path';

export class CreateWindow {
    CreateWindow = (url: string, show = true) => {
        app.whenReady().then(async () => {
            console.log('createWindow ready');
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
            win.webContents.openDevTools();

            // キャッシュクリア
            await win.webContents.session.clearCache();

            win.once('ready-to-show', () => {
                if (show) win.show();
            });

            try {
                console.log(`start loda url:${url}`);
                win.loadURL(url);
            } catch (err) {
                const errMsg = err instanceof Error ? (err.stack ?? err.message) : String(err);
                const msg = `loadURL failed url=${url} err=${errMsg}`;
                CreateLog.WriteLog(msg);
            }

            win.webContents.on('did-fail-load', (event, errorCode, errorDescription, validatedURL, isMainFrame) => {
                const msg = `did-fail-load url=${validatedURL} code=${errorCode} desc=${errorDescription} isMainFrame=${isMainFrame}`;
                CreateLog.WriteLog(msg);
            });

            return win;
        });
    }
}