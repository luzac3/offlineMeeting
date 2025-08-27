import { SignalR } from "@root/share/SignalR";
export class SendQuiz {
    SignalR: SignalR;
    constructor(signalR: SignalR) {
        this.SignalR = signalR;
        this.sendQuizId();
    }

    private sendQuizId = () => {
        const sendQuizElement = document.getElementById("send_quiz");
        if (sendQuizElement != null) {
            sendQuizElement.onclick = () => {
                const quizId = (<HTMLInputElement>document.getElementById("quiz_id")).value;
                this.SignalR.send("SendQuizId", quizId);
            }
        }
    }
}