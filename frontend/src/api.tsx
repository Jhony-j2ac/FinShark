import axios from "axios"
import { CompanyBalanceSheet, CompanyCashFlow, CompanyCompData, CompanyIncomeStatement, CompanyKeyMetrics, CompanyProfile, CompanySearch, CompanyTenK } from "./company"
import { isAxiosError } from "axios";

interface searchResponse {
    data: CompanySearch[];
}


export const searchCompanies  = async (query: string ) => {
    try{
        const data = await axios
            .get<searchResponse>(`https://financialmodelingprep.com/api/v3/search?query=${query}&apikey=${process.env.REACT_APP_API_KEY}`);
        return data;
    }catch (error) {
        if(isAxiosError(error)){
            console.log("error message: " + error.message);
            return error.message;
        }else{
            console.log("unexpected error :" + error);
            return "unexpected error :" + error;
        }
    }
}

export const getCompanyProfile = async (query: string) => {
    try {
        const data = await  axios.get<CompanyProfile[]>(
             `https://financialmodelingprep.com/api/v3/profile/${query}?apikey=${process.env.REACT_APP_API_KEY}`
        )
        return data;
    } catch (error : any) {
        console.error("error message from the API" + error.message)
    }
}


export const getKeyMetrics = async (query: string) => {
    try {
        const data = await  axios.get<CompanyKeyMetrics[]>(
             `https://financialmodelingprep.com/api/v3/key-metrics-ttm/${query}?apikey=${process.env.REACT_APP_API_KEY}`
        )
        return data;
    } catch (error : any) {
        console.error("error message from the API" + error.message)
    }
}

export const getIncomeStatement = async (query: string) =>{
    try{
        const data = axios.get<CompanyIncomeStatement[]>(
            `https://financialmodelingprep.com/api/v3/income-statement/${query}?apikey=${process.env.REACT_APP_API_KEY}`
        )
        return data;
    } catch (error: any){
        console.error("error message from the API" + error.message)
    }
}


export const getBalanceSheet = async (query: string) =>{
    try{
        const data = axios.get<CompanyBalanceSheet[]>(
            `https://financialmodelingprep.com/api/v3/balance-sheet-statement/${query}?apikey=${process.env.REACT_APP_API_KEY}`
        )
        return data;
    } catch (error: any){
        console.error("error message from the API" + error.message)
    }
}


export const getChashFlow = async (query: string) =>{
    try{
        const data = axios.get<CompanyCashFlow[]>(
            `https://financialmodelingprep.com/api/v3/cash-flow-statement/${query}?apikey=${process.env.REACT_APP_API_KEY}`
        )
        return data;
    } catch (error: any){
        console.error("error message from the API" + error.message)
    }
}



export const getCompData = async (query: string) =>{
    try{
        const data = axios.get<CompanyCompData[]>(
            `https://financialmodelingprep.com/api/v3/stock_peers/${query}?apikey=${process.env.REACT_APP_API_KEY}`
        )
        return data;
    } catch (error: any){
        console.error("error message from the API" + error.message)
    }
}


export const getTenK = async (query: string) =>{
    try{
        const data = axios.get<CompanyTenK[]>(
            `https://financialmodelingprep.com/api/v3/sec_filings/${query}?type=10-k&page=0&&apikey=${process.env.REACT_APP_API_KEY}`
        )
        return data;
    } catch (error: any){
        console.error("error message from the API" + error.message)
    }
}

