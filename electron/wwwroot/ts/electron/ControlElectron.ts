export class ControlElectron {
    { app, BrowserWindow, ipcMain } = require('electron');
    path = require('path');
    fs: FileSystem = require('fs');

    // 開発時の ASP.NET URL を環境変数で指定 (例: APP_URL=http://localhost:5000)
    APP_URL: string = process.env.APP_URL || 'http://localhost:5000';
    constructor() {
        app.whenReady().then(createMainWindow);

        app.on('window-all-closed', () => {
            if (process.platform !== 'darwin') app.quit();
        });
    }


    createMainWindow = () => {
        const win = new BrowserWindow({
            width: 1200,
            height: 800,
            webPreferences: {
                preload: path.join(__dirname, 'preload.js'),
                contextIsolation: true
            }
        });
        win.loadURL(APP_URL);
}

    // シンプルな HTML エスケープ
    escapeHtml = s => {
        if (s == null) return '';
        return String(s)
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#39;');
}

    ipcMain.handle('print-with-template', async (event, args) => {
        try {
            const { template, apiPath, headers } = args;
            // 絶対 URL か相対パスか判定
            const apiUrl = apiPath.startsWith('http') ? apiPath : `${APP_URL.replace(/\/$/, '')}/${apiPath.replace(/^\//, '')}`;

            // Node (Electron) の fetch を使用（Node 18+ の場合）
            const res = await fetch(apiUrl, { headers: headers || {} });
            if (!res.ok) {
                return { ok: false, reason: `HTTP ${res.status}` };
            }
            const data = await res.json();

            // テンプレート読み込み
            const templatePath = path.join(__dirname, 'templates', template);
            let tpl = fs.readFileSync(templatePath, 'utf8');

            // 単純置換: object のプロパティを {{Key}} にマッピング
            for (const key of Object.keys(data)) {
                if (Array.isArray(data[key])) {
                    // 例: Items の配列を HTML に変換（テンプレート内で {{Items}} を使えるように）
                    const itemsHtml = data[key].map(item => {
                        // item はオブジェクトを仮定
                        // カラムを合成する簡易例。必要に応じてテンプレートを拡張してください。
                        return `<div class="row">
                        <span class="name">${escapeHtml(item.Name)}</span>
                        <span class="qty">${escapeHtml(item.Qty)}</span>
                        <span class="price">${escapeHtml(item.Price)}</span>
                      </div>`;
                    }).join('');
                    tpl = tpl.replace(new RegExp(`{{\\s*${key}\\s*}}`, 'g'), itemsHtml);
                } else {
                    tpl = tpl.replace(new RegExp(`{{\\s*${key}\\s*}}`, 'g'), escapeHtml(data[key]));
                }
            }

            // 残ったプレースホルダを空文字に
            tpl = tpl.replace(/{{\s*[\w\.]+\s*}}/g, '');

            // 非表示ウィンドウで読み込んで印刷
            const printWindow = new BrowserWindow({
                show: false,
                webPreferences: {
                    contextIsolation: true
                }
            });

            printWindow.loadURL('data:text/html;charset=utf-8,' + encodeURIComponent(tpl));

            return await new Promise((resolve) => {
                printWindow.webContents.on('did-finish-load', () => {
                    // CSS 読み込みなどを待つため少し遅延
                    setTimeout(() => {
                        printWindow.webContents.print({ silent: true }, (success, failureReason) => {
                            printWindow.close();
                            if (success) resolve({ ok: true });
                            else resolve({ ok: false, reason: failureReason });
                        });
                    }, 250);
                });

                printWindow.webContents.on('did-fail-load', () => {
                    printWindow.close();
                    resolve({ ok: false, reason: 'did-fail-load' });
                });
            });
        } catch (err) {
            return { ok: false, reason: err && err.message };
        }
    });
}
