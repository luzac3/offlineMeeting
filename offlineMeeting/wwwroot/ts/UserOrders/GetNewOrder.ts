import { SignalR } from "@root/share/SignalR";
import { FetchApi } from "@root/share/FetchApi"
import { PrintOrder } from "./PrintOrder";

export class GetNewOrder {
    private url: string;
    private method: string;
    private headers: { [key: string]: string };
    private responseKind: string;

    constructor() {
        const url = new URL(window.location.href);
        const params = new URLSearchParams(url.search);

        if (params.has('eid')) {
            this.url = `/Pos/UserOrdersPartial?eid=${params.get('eid')}`;
        } else {
            this.url = '/Pos/UserOrdersPartial';
        }
        this.method = 'GET';

        this.headers = {
            "Content-Type": "application/json",
            'X-Requested-With': 'XMLHttpRequest'
        };
        this.responseKind = "text";
    }

    GetNewOrder = (signalR: SignalR, ctx: AudioContext) => {
        signalR.get("NewOrder", async (data) => {
            const parsedData = <{ [key: string]: string }>data;
            const orderTableEntityList = JSON.parse(parsedData.orderTableEntityList) as { [key: string]: string }[];
            const resultIds = orderTableEntityList.map(x => x.ResultId);

            this.send().then((orderdata) => {
                this.updateTableBody(orderdata);
                PrintOrder.RequestElectronPrint(resultIds);
            });
        });
    }

    private updateTableBody(newHtml: string): void {
        const table = document.getElementById("user_order_list");
        if (!table) return;

        const thead = table.querySelector('thead');
        if (!thead) return;

        const tbodies = table.querySelectorAll('tbody');
        tbodies.forEach(tbody => tbody.remove());

        thead.insertAdjacentHTML('afterend', newHtml);
    }

    private send = async () => {
        const fetchApi = new FetchApi();

        return await fetchApi.send(
            this.url,
            this.method,
            this.headers,
            null,
            this.responseKind
        ).then(async (data: string) => {
            return data;
        }).catch(e => {
            throw e;
        });
    }
}