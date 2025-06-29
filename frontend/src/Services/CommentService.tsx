import { useAuthHttp } from "../Context/useAuthHttp";
import { ErrorHandler } from "../Helpers/ErrorHandler";
import { CommentGet, CommentPost } from "../Models/Comment";

const api = "https://localhost:44351/api/comment/"
export const useCommentService = () => {
    const {authAxios } = useAuthHttp();
    const commentPostAPI = async (title: string, content: string, symbol: string) => {
        try{
            const data = await authAxios.post<CommentPost>(api + symbol , {
                title,
                content
            } ,
            {
                withCredentials: true 
            });
            return data;

        }catch(e){
            ErrorHandler(e);
        }
    }

    const commentGetAPI = async (symbol: string) => {
        try{
            const data = await authAxios.get<CommentGet[]>(api, {
                params: {
                    Symbol: symbol
                },
                withCredentials: true
            });
            return data;

        }catch(e){
            ErrorHandler(e);
        }
    }

    return { commentPostAPI, commentGetAPI };
}