import { SignalR } from "@root/share/SignalR";

export class GetQuiz {
    private url: string;
    private method: string;
    private headers: { [key: string]: string };

    constructor() {
        this.url = '/Quiz/GetQuiz';
        this.method = 'Get';
        this.headers = {
            "Content-Type": "application/json",
            'X-Requested-With': 'XMLHttpRequest'
        };
    }

    GetQuiz = async (signalR: SignalR) => {
        signalR.get("ReceiveQuizId", async (quizId) => {
            const parsedQuizId = <{ [key: string]: string }>quizId;

            const response = await fetch(this.url + `/${parsedQuizId.QuizId}`, {
                method: this.method,
                headers: this.headers
            });

            await response.text().then(data => {
                console.log(data);
                document.getElementById("quiz")!.innerHTML = data;
            });
            // 問題を受信したらボタンをアクティブに
            const sendElement = document.getElementById('send') as HTMLButtonElement;
            sendElement.disabled = false;
        });
    }
}