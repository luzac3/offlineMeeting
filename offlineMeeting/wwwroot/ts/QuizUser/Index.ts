import { SignalR } from "@root/share/SignalR";
import { BackButton } from "@root//share/BackButton";
import { BlurImage } from "@root//share/BlurImage";
import { SendAnswer } from "./SendAnswer";
import { Canvas } from "./Canvas";
import { GetQuiz } from "./GetQuiz";

(() => {
    const signalR = new SignalR();
    signalR.activate();
    new Canvas();
    const getQuiz = new GetQuiz();
    const sendAnswer = new SendAnswer();
    const backButton = new BackButton();
    sendAnswer.SendCanvas();
    getQuiz.GetQuiz(signalR);
    backButton.Buck();
    BlurImage.loaded();
})();