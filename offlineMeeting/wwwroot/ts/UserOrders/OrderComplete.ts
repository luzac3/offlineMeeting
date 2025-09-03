import { FetchApi } from "@root/share/FetchApi"
import { SetEventListner } from "@root/share/SetEventListner";

export class OrderComplete {
    private url: string;
    private method: string;
    private headers: { [key: string]: string };
    private responseKind: string;
    private parentElement: HTMLElement | null = document.getElementById("user_order_list");

    constructor() {
        this.url = '/Pos/OrderComplete';
        this.method = 'POST';
        this.headers = {
            "Content-Type": "application/json",
            'X-Requested-With': 'XMLHttpRequest'
        };
        this.responseKind = "text";

        this.Batch();
        this.Complete();
    }

    private Batch = () => {
        const batchButtonElement = document.getElementById("batch_processing") as HTMLButtonElement;
        batchButtonElement.addEventListener("click", () => {
            const checkedResultElements = Array.from(
                this.parentElement?.querySelectorAll('.completed_check[type="checkbox"]:checked + input') ?? []
            );
            const checkedResultIdArray = checkedResultElements.map(el => (<HTMLInputElement>el).value);

            this.send({ "ResultIdList": checkedResultIdArray }).then((data) => {
                this.OrderCompleted(data, checkedResultElements as HTMLElement[]);
            });
        });
    }

    private Complete = () => {
        SetEventListner.setEvent(this.parentElement, 'click', '.completed', (e: Event) => {
            const target = e.target as HTMLElement;
            const resultId = (<HTMLInputElement>target.nextElementSibling).value;
            this.send({ "ResultIdList": [resultId]}).then((data) => {
                this.OrderCompleted(data, [target.parentElement as HTMLElement]);
            });
        });
    }

    private send = async (resultIds: {[key: string]: string[]}) => {
        const fetchApi = new FetchApi();

        return await fetchApi.send(
            this.url,
            this.method,
            this.headers,
            resultIds,
            this.responseKind
        ).then(async (data: string) => {
            return data;
        }).catch(e => {
            throw e;
        });
    }

    private OrderCompleted = (data: string, elements:  HTMLElement[]) => {
        const result = JSON.parse(data) as { [key: string]: string };
        if (result.status == "200") {
            elements.forEach(element => {
                const tr = element.closest("tr");
                if (tr) {
                    tr.remove();
                }
            });
        }
        window.alert(result.message);
    }
}