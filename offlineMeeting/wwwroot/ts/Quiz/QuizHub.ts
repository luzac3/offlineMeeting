import { SignalR } from "@root/share/SignalR";
export class QuizHub {
    SignalR: SignalR;
    constructor(signalR: SignalR) {
        this.SignalR = signalR;
        const sendQuizElement = document.getElementById("send_quiz");
        if (sendQuizElement != null) {
            sendQuizElement.onclick = () => {
                const quizId = (<HTMLInputElement>document.getElementById("quiz_id")).value;
                signalR.send("SendQuizId", quizId);
            }
        }
    }
}