import { CreateWindow } from './CreateWindow';
import { Printer } from './Printer';
import { ipcMain } from 'electron';
import { GetSetting } from '../share/GetSetting';

export class SetElectron {
    constructor() {

        console.log('start set electron');
        const data = GetSetting.Setting();

        const createWindow = new CreateWindow();
        createWindow.CreateWindow(data.url);

        ipcMain.handle('printReceipt', (event, args) => {
            args.resultIds.forEach((resultId: string) => {
                const printer = new Printer(resultId);
                printer.Print();
            });
        });
    }
}