import axios from "axios";
import { ErrorHandler } from "../Helpers/ErrorHandler";
import { CommentGet, CommentPost } from "../Models/Comment";

const api = "https://localhost:44351/api/comment/"
export const commentPostAPI = async (title: string, content: string, symbol: string) => {
    try{
        const data = await axios.post<CommentPost>(api + symbol , {
            title,
            content
        } );
        return data;

    }catch(e){
        ErrorHandler(e);
    }
}

export const commentGetAPI = async (symbol: string) => {
    try{
        const data = await axios.get<CommentGet[]>(api, {
            params: {
                Symbol: symbol
            }
        });
        return data;

    }catch(e){
        ErrorHandler(e);
    }
}