import path from 'path';
import fs from 'fs';

export class GetSetting {
    static Setting = () => {
        const isDev = process.env.NODE_ENV === 'development' || process.defaultApp;
        const settingPath = isDev
            ? path.join(__dirname, 'appData/json/setting.json')
            : path.join(path.dirname(process.execPath), 'setting.json');
        const bufferData = fs.readFileSync(settingPath);
        return JSON.parse(bufferData.toString());
    }
}