import { SignalR } from "@root/share/SignalR";
import { ControlNavbar } from "@root/share/ControlNavbar";
import { BackButton } from "@root//share/BackButton";
import { SendAnswer } from "./SendAnswer";
import { Canvas } from "./Canvas";
import { GetQuiz } from "./GetQuiz";

(() => {
    const signalR = new SignalR();
    signalR.activate();
    new ControlNavbar();
    new Canvas();
    const getQuiz = new GetQuiz();
    const sendAnswer = new SendAnswer();
    const backButton = new BackButton();
    sendAnswer.SendCanvas();
    getQuiz.GetQuiz(signalR);
    backButton.Buck();
})();