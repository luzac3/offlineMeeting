import { SignalR } from "@root/share/SignalR";
import { ControlNavbar } from "@root/share/ControlNavbar";
import { SendAnswer } from "./SendAnswer";
import { Canvas } from "./Canvas";
import { QuizHub } from "./QuizHub";
import { GetQuiz } from "./GetQuiz";

(() => {
    const signalR = new SignalR();
    signalR.activate();
    new ControlNavbar();
    new Canvas();
    new QuizHub(signalR);
    const sendAnswer = new SendAnswer();
    const getQuiz = new GetQuiz();
    sendAnswer.SendCanvas();
    getQuiz.GetQuiz(signalR);
})();