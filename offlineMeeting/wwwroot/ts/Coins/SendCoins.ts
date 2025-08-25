import { FetchApi } from "@root/share/FetchApi"

export class SendCoins {
    private url: string;
    private method: string;
    private headers: { [key: string]: string }; 
    private responseKind: string;

    constructor() {
        this.url = '/Coins/PaymentComplete';
        this.method = 'POST';
        this.headers = {
            "Content-Type": "application/json",
            'X-Requested-With': 'XMLHttpRequest'
        };
        this.responseKind = "text";
    }

    setCoinsData = (value: number, coins: number, details: any) => {
        let coinsEntity: { [key: string]: number } = {
            coins: coins,
            value: value
        };

        this.send(coinsEntity).then((data: string) => {
            console.log(details);
            console.log(data);
            // details.payer.name.given_name
            alert('支払いが完了しました');
        });
    }

    private send = async (coinsEntity: { [key: string]: number }) => {
        const fetchApi = new FetchApi();

        return await fetchApi.send(
            this.url,
            this.method,
            this.headers,
            coinsEntity,
            this.responseKind
        ).then(async (data: string) => {
            return data;
        }).catch(e => {
            throw e;
        });
    }
}