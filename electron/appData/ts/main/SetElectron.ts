import { CreateWindow } from './CreateWindow';
import { Printer } from './Printer';
import { ipcMain } from 'electron';
import { GetSetting } from '../share/GetSetting';
import path from 'path';

export class SetElectron {
    constructor() {
        console.log('start set electron');
        const data = GetSetting.Setting();
        const createWindow = new CreateWindow();

        // メニュー項目を構築
        const menuItems: { name: string; url: string }[] = [];
        if (data.orderUrl && data.orderName) {
            menuItems.push({ name: data.orderName, url: data.orderUrl });
        }
        if (data.checkinUrl && data.checkinName) {
            menuItems.push({ name: data.checkinName, url: data.checkinUrl });
        }

        // メニュー画面を表示
        const isDev = process.env.NODE_ENV === 'development' || process.defaultApp;
        const menuPath = isDev
            ? path.join(__dirname, 'appData/menu.html')
            : path.join(path.dirname(process.execPath), 'config', 'menu.html');
        createWindow.CreateWindowFromFile(menuPath);

        // メニュー項目取得
        ipcMain.handle('getMenuItems', () => {
            return menuItems;
        });

        // メニューからページを開く
        ipcMain.handle('openPage', (_event, url: string) => {
            createWindow.CreateWindow(url);
        });

        // 注文印刷
        ipcMain.handle('printReceipt', (_event, args) => {
            args.resultIds.forEach((resultId: string) => {
                const printer = new Printer(resultId, data.printUrlBase);
                printer.Print();
            });
        });

        // チェックインQR印刷
        ipcMain.handle('printCheckinQr', (_event, args) => {
            const printer = new Printer(args.userCd, data.checkinPrintUrlBase);
            printer.Print();
        });
    }
}