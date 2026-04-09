import { CreateLog } from '../share/CreateLog'
import { app, BrowserWindow } from 'electron'
import path from 'path';

export class CreateWindow {
    CreateWindow = (url: string, show = true): Promise<BrowserWindow> => {
        return app.whenReady().then(async () => {
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

            // キャッシュクリア
            await win.webContents.session.clearCache();

            // 開発時のみDevToolsを開く例
            if (process.env.NODE_ENV === 'development') {
                win.webContents.openDevTools({ mode: 'detach' });
            }

            win.once('ready-to-show', () => {
                if (show) win.show();
            });

            try {
                console.log(`start load url:${url}`);
                await win.loadURL(url);
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

    CreateWindowFromFile = (filePath: string, show = true): Promise<BrowserWindow> => {
        return app.whenReady().then(async () => {
            const win = new BrowserWindow({
                width: 600,
                height: 500,
                show: false,
                webPreferences: {
                    preload: path.join(__dirname, "preload.js"),
                    contextIsolation: true,
                    nodeIntegration: false
                }
            });

            win.once('ready-to-show', () => {
                if (show) win.show();
            });

            try {
                await win.loadFile(filePath);
            } catch (err) {
                const errMsg = err instanceof Error ? (err.stack ?? err.message) : String(err);
                CreateLog.WriteLog(`loadFile failed path=${filePath} err=${errMsg}`);
            }

            return win;
        });
    }
}