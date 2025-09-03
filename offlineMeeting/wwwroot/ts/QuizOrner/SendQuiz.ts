import { SignalR } from "@root/share/SignalR";
export class SendQuiz {
    SignalR: SignalR;
    private url: string;
    private method: string;
    private headers: { [key: string]: string };

    constructor(signalR: SignalR) {
        this.url = '/Quiz/SendQuiz';
        this.method = 'Get';
        this.headers = {
            "Content-Type": "application/json",
            'X-Requested-With': 'XMLHttpRequest'
        };
        this.SignalR = signalR;
        this.sendQuizId();
    }

    private sendQuizId = () => {
        const sendQuizElement = document.getElementById("send_quiz");
        if (sendQuizElement != null) {
            sendQuizElement.onclick = async () => {
                const quizId = (<HTMLInputElement>document.getElementById("quiz_id")).value;
                const response = await fetch(this.url + `/${quizId}`, {
                    method: this.method,
                    headers: this.headers
                });

                await response.text().then(data => {
                    const result = JSON.parse(data);
                    if (result.status == 200 || "OK") {
                        window.alert(result.message);
                    } else {
                        window.alert(result.message);
                    }
                });
            }
        }
    }
}