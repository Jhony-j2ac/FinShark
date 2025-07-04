import React, { SyntheticEvent, useEffect, useState } from 'react'
import { CompanySearch } from '../../company';
import { searchCompanies } from '../../api';
import Navbar from '../../Components/Navbar/Navbar';
import Search from '../../Components/Search/Search';
import ListPortfolio from '../../Components/Portfolio/ListPortfolio/ListPortfolio';
import CardList from '../../Components/CardList/CardList';
import { PortfolioGet } from '../../Models/Porfolio';
// Adjust the import path to match the actual file location and casing
import { toast } from 'react-toastify';
import { usePortfolioService } from '../../Services/PortfolioServices';



interface Props {

}

const SearchPage : React.FC = (props: Props) : JSX.Element => {
  const { portfolioAddAPI, portfolioDeleteAPI, portfolioGetAPI } = usePortfolioService();
  const [search, setSearch] = useState<string>("");
  const [searchResult, setSearchResult] = useState<CompanySearch[]>([]);
  const [serverError, setServerError] = useState<string>("");
  const [portfolioValues, setPortfolioValues] = useState<PortfolioGet[] | null>([]);

    useEffect(() => {
        getPortfolio();
    }, []);

  const saveResult = (result: CompanySearch[]) =>{
    setSearchResult(result);
  }


  const handleSearchChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSearch(e.target.value);
    console.log(e);
  }

  const getPortfolio  = () => {
    portfolioGetAPI().then((res : any) => {
        if(res?.data){
            setPortfolioValues(res.data);
        }
    }).catch((err : any) => {
        toast.warning("Could not get portfolio values");
    });
  }

  const onSearchSubmit =  async (e : SyntheticEvent) => {
    e.preventDefault();
    const result = await searchCompanies(search);
    if(typeof result === "string"){
      setServerError(result);
    }else if(typeof result !== "undefined" && Array.isArray(result?.data)){
      setSearchResult(result.data);
    }
    console.log(searchResult);
  }

  const onPortfolioCreate = (e: any) => {
     e.preventDefault();
     const value = e.target[0].value;
     portfolioAddAPI(value).then((res : any) => {
        if(res?.status === 204 || res?.status === 200){
            toast.success("Added to portfolio successfully");
            getPortfolio();
        }
     }).catch((err : any) => {
        toast.warning("Could not create portfolio ");
    });
  }

  const onPortfolioDelete = (e: any) =>{
    e.preventDefault();
    const value =  e.target[0].value;
    portfolioDeleteAPI(value).then((res) => {
        if(res?.status === 200){
            toast.success("Deleted portfolio successfully");
            getPortfolio();
        }
     }).catch((err : any) => {
        toast.warning("Could not create portfolio ");
    });
  }
  return (
    <>
      <Search onSearchSubmit={onSearchSubmit} handleSearchChange={handleSearchChange} search={search} ></Search>
      <ListPortfolio PortfolioValues={portfolioValues!} onPortfolioDelete={onPortfolioDelete}></ListPortfolio>
      {serverError && <h1>{serverError}</h1>}
      <CardList searchResult={searchResult} onPortfolioCreate={onPortfolioCreate}></CardList>
    </>
  )
}

export default SearchPage