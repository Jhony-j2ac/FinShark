import axios from "axios"
import { toast } from "react-toastify";

export const ErrorHandler = (error: any) => {
    if(axios.isAxiosError(error)){
        if (typeof error.response != "undefined" && error.response != null) {
            var err = error.response;
            if(Array.isArray(err?.data.errors)){
                for(let val of err?.data.errors){
                    toast.warning(val.description)
                }
            }else if(typeof err?.data.errors === 'object'){
                for(let e in err?.data.errors){
                    toast.warning(e[0])
                }
            }else if(err?.data){
                toast.warning(err.data)
            }else if(err?.status === 401){
                toast.warning("Please login");
                window.history.pushState({}, "LoginPage", "/login");
            }else if(err){
                toast.warning(error?.message);
            }
        }else {
            toast.warning(error.message);
        } 
        
    }
}