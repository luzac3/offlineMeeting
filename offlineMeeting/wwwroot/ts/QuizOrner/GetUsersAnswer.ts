import { SignalR } from "@root/share/SignalR";

export class GetUsersAnswer {
    private url: string;
    private method: string;
    private headers: { [key: string]: string };

    constructor() {
        this.url = '/Quiz/GetAnswer';
        this.method = 'Get';
        this.headers = {
            "Content-Type": "application/json",
            'X-Requested-With': 'XMLHttpRequest'
        };
    }

    GetAnswer = async (signalR: SignalR) => {
        signalR.get("UserAnswerId", async (result) => {
            const answerListElement = document.getElementById("answer_list");
            const parsedResultId = <{ [key: string]: string }>result;

            const response = await fetch(this.url + `/${parsedResultId.ResultId}`, {
                method: this.method,
                headers: this.headers
            });

            if (answerListElement != null) {
                await response.text().then(data => {
                    answerListElement.insertAdjacentHTML('beforeend', data);
                });
            }
        });
    }
}