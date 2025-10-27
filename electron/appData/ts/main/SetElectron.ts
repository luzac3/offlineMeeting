import { CreateWindow } from './CreateWindow';
import { Printer } from './Printer';
import { ipcMain } from 'electron';
import fs from 'fs';

export class SetElectron {
    constructor() {

        console.log('start set electron');
        const bufferData = fs.readFileSync('./appData/json/setting.json');
        const data = JSON.parse(bufferData.toString());

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