import { FetchApi } from "@root/share/FetchApi";

export class Checkin {
    setButtons = () => {
        const rows = document.querySelectorAll<HTMLElement>('.joiner_row');
        rows.forEach((row) => {
            const btn = row.querySelector('.btn_checkin') as HTMLButtonElement;
            if (!btn) return;

            // 既にチェックイン済みの場合はスキップ
            if (btn.classList.contains('checked_in')) {
                this.setReprintButton(row);
                return;
            }

            btn.addEventListener('click', async () => {
                const id = row.dataset.id;
                const userCd = row.dataset.usercd;
                const userName = row.dataset.username;
                const qrUrl = row.dataset.qrurl;

                if (!id || !userCd || !userName || !qrUrl) return;

                btn.disabled = true;
                btn.textContent = '処理中...';

                try {
                    const fetchApi = new FetchApi();
                    const result = await fetchApi.send(
                        '/QR/Checkin',
                        'POST',
                        { "Content-Type": "application/json" },
                        { Id: Number(id), UserCd: Number(userCd), UserName: userName, QrUrl: qrUrl }
                    ) as { success: boolean };

                    if (result?.success) {
                        // 完了表示・再印刷ボタンはサーバーのCheckinComplete通知(SignalR)で切り替える
                        btn.textContent = '処理中...';
                    } else {
                        btn.textContent = '失敗';
                        btn.disabled = false;
                    }
                } catch {
                    btn.textContent = 'エラー';
                    btn.disabled = false;
                }
            });
        });
    }

    private setReprintButton = (row: HTMLElement) => {
        const reprintBtn = row.querySelector('.btn_reprint') as HTMLButtonElement;
        if (!reprintBtn) return;

        reprintBtn.addEventListener('click', () => {
            const userCd = reprintBtn.dataset.usercd;
            if (userCd && window.electronAPI && window.electronAPI.printCheckinQr) {
                window.electronAPI.printCheckinQr({ userCd });
            }
        });
    }
}