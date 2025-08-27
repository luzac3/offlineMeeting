import { SignalR } from "@root/share/SignalR";
import { ControlNavbar } from "@root/share/ControlNavbar";
import { SendQuiz } from "./SendQuiz";
import { SendUserPoints } from "./SendUserPoints";
import { GetUsersAnswer } from "./GetUsersAnswer";

(() => {
    const signalR = new SignalR();
    signalR.activate();
    new ControlNavbar();
    new SendQuiz(signalR);
    const getUsersAnswer = new GetUsersAnswer();
    const sendUserPoints = new SendUserPoints();
    getUsersAnswer.GetAnswer(signalR);
    sendUserPoints.SendPoints();
})();