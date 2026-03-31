import { FetchApi } from "@root/share/FetchApi";

export class Checkin {
    setButtons = () => {
        const rows = document.querySelectorAll<HTMLElement>('.joiner_row');
        rows.forEach((row) => {
            const btn = row.querySelector('.btn_checkin') as HTMLButtonElement;
            if (!btn) return;

            btn.addEventListener('click', async () => {
                const userCd = row.dataset.usercd;
                const userName = row.dataset.username;

                if (!userCd || !userName) return;

                btn.disabled = true;
                btn.textContent = '処理中...';

                try {
                    const fetchApi = new FetchApi();
                    const result = await fetchApi.send(
                        '/QR/Checkin',
                        'POST',
                        { "Content-Type": "application/json" },
                        { UserCd: Number(userCd), UserName: userName }
                    ) as { success: boolean };

                    if (result?.success) {
                        btn.textContent = '✓ 完了';
                        btn.classList.add('checked_in');
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
}