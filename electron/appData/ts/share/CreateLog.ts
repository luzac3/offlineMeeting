import path from 'path';
import fs from 'fs';

export class CreateLog {
    constructor() {
        this.OutputWholeError();
    }

    static WriteLog = async (message: string) => {
        try {
            const file = this.MakeLogFilePath();
            const dir = path.dirname(file);
            await fs.promises.mkdir(dir, { recursive: true });
            const timestamp = new Date().toISOString();
            const entry = `[${timestamp}] ${message}\n`;
            await fs.promises.appendFile(file, entry, 'utf8');
        } catch (e) {
            // ログ自体の失敗はコンソールに出す（ここで例外を投げない）
            // eslint-disable-next-line no-console
            console.error('Failed to write log', e);
        }
    }

    private static MakeLogFilePath = () => {
        const now = new Date();
        const y = now.getFullYear();
        const m = CreateLog.pad(now.getMonth() + 1);
        const d = CreateLog.pad(now.getDate());
        const hh = CreateLog.pad(now.getHours());
        const mm = CreateLog.pad(now.getMinutes());
        const ss = CreateLog.pad(now.getSeconds());
        const fileName = `${y}${m}${d}-${hh}-${mm}-${ss}.log`;
        const dir = path.resolve(process.cwd(), 'wwwroot', 'log');
        return path.join(dir, fileName);
    }

    private static pad(n: number) { return String(n).padStart(2, '0'); }

    private OutputWholeError = () => {
        // グローバル未捕捉エラーもログに落とす（main プロセス全体の安全ネット）
        process.on('uncaughtException', (err) => {
            // eslint-disable-next-line no-console
            console.error('uncaughtException', err);
            void CreateLog.WriteLog(`uncaughtException: ${err && err.stack ? err.stack : String(err)}`);
        });

        process.on('unhandledRejection', (reason) => {
            // eslint-disable-next-line no-console
            console.error('unhandledRejection', reason);
            void CreateLog.WriteLog(`unhandledRejection: ${String(reason)}`);
        });
    }
}