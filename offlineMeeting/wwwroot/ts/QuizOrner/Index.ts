import { SignalR } from "@root/share/SignalR";
import { BackButton } from "@root//share/BackButton";
import { SendQuiz } from "./SendQuiz";
import { SendUserPoints } from "./SendUserPoints";
import { GetUsersAnswer } from "./GetUsersAnswer";

(() => {
    const signalR = new SignalR();
    signalR.activate();
    new SendQuiz(signalR);
    const getUsersAnswer = new GetUsersAnswer();
    const sendUserPoints = new SendUserPoints();
    const backButton = new BackButton();
    getUsersAnswer.GetAnswer(signalR);
    sendUserPoints.SendPoints();
    backButton.Buck();
})();