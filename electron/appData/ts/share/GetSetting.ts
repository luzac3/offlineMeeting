import path from 'path';
import fs from 'fs';

export class GetSetting {
    static Setting = () => {
        const isDev = process.env.NODE_ENV === 'development' || process.defaultApp;
        const prodPath = path.join(path.dirname(process.execPath), 'config', '/setting.json');
        const devPath = path.join(__dirname, 'appData/json/setting.json');
        const settingPath = isDev ? devPath : prodPath;
        const isRaspberryPi = false;
        try {
            const bufferData = fs.readFileSync(settingPath);
            return JSON.parse(bufferData.toString());
        } catch (error) {
            console.error(`Failed to read setting file at: ${settingPath}`, error);
            // ファイルが存在しない/読み取りエラーの場合のデフォルト設定を返す
            return { /* default settings */ };
        }
    }
}