import { SignalR } from "@root/share/SignalR";
import { ControlNavbar } from "@root/share/ControlNavbar";
import { SendCanvasImage } from "./SendCanvasImage";
import { Canvas } from "./Canvas";
import { QuizHub } from "./QuizHub";
import { GetQuiz } from "./GetQuiz";

(() => {
    const signalR = new SignalR();
    signalR.activate();
    new ControlNavbar();
    new Canvas();
    new QuizHub(signalR);
    const sendCanvasImage = new SendCanvasImage();
    const getQuiz = new GetQuiz();
    sendCanvasImage.SendCanvas();
    getQuiz.GetQuiz(signalR);
})();