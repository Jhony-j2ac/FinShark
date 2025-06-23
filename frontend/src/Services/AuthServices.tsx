import axios from "axios"
import { ErrorHandler } from "../Helpers/ErrorHandler";
import { UserProfileToken } from "../Models/User";

const api = "https://localhost:44351/api/"

export const loginAPI = async (username: string, password : string) => {
    try{
        const data = await axios.post<UserProfileToken>(api + "Account/login", {
            userName : username,
            password
        })
        return data;
    }catch(e){
        ErrorHandler(e);
    }
}

export const registerAPI = async (email : string, username: string, password : string) => {
    try{
        const data = await axios.post<UserProfileToken>(api + "Account/register", {
            email,
            username,
            password
        })
        return data;
    }catch(e){
        ErrorHandler(e);
    }
}