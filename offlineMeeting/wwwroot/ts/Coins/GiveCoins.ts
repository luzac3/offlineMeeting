import { FetchApi } from "@root/share/FetchApi"

export class GiveCoins {
    private url: string;
    private method: string;
    private headers: { [key: string]: string }; 
    private responseKind: string;

    constructor() {
        this.url = '/Coins/GiveCoins';
        this.method = 'POST';
        this.headers = {
            "Content-Type": "application/json",
            'X-Requested-With': 'XMLHttpRequest'
        };
        this.responseKind = "text";
    }

    setCoinsData = () => {
        const forUserCdElement = document.getElementById("for_user_cd") as HTMLSelectElement;
        const counterNumberElement = document.getElementsByClassName("counter")[0].querySelector(".counter_number") as HTMLInputElement;
        const giveElement = document.getElementById("give") as HTMLButtonElement;

        giveElement.onclick = () => {
            const giveCoinsEntity: { [key: string]: number } = {
                ForUserCd: Number(forUserCdElement.value),
                Coins: Number(counterNumberElement.value),
                Value: 0
            };

            if (giveElement != null) {
                const coinsRate = Math.round(Number(counterNumberElement.value) * 0.2) == 0 ? 1 : Math.round(Number(counterNumberElement.value) * 0.2);
                if (window.confirm(
                    "コイン" + coinsRate + "枚が手数料として引かれます。よろしいですか？"
                )) {
                    this.send(giveCoinsEntity).then((data: string) => {
                        const response = JSON.parse(data);
                        console.log(response.StatusCode);
                        if (response.StatusCode == "200" || "OK") {
                            window.alert('譲渡が完了しました');
                        } else {
                            window.alert('譲渡に失敗しました。' + response.Message);
                        }
                    });
                }
            }
        }
    }

    private send = async (giveCoinsEntity: { [key: string]: number }) => {
        const fetchApi = new FetchApi();

        return await fetchApi.send(
            this.url,
            this.method,
            this.headers,
            giveCoinsEntity,
            this.responseKind
        ).then(async (data: string) => {
            return data;
        }).catch(e => {
            throw e;
        });
    }
}