import React, { SyntheticEvent, useState } from 'react'
import { CompanySearch } from '../../company';
import { searchCompanies } from '../../api';
import Navbar from '../../Components/Navbar/Navbar';
import Search from '../../Components/Search/Search';
import ListPortfolio from '../../Components/Portfolio/ListPortfolio/ListPortfolio';
import CardList from '../../Components/CardList/CardList';



interface Props {

}

const SearchPage : React.FC = (props: Props) : JSX.Element => {
  const [search, setSearch] = useState<string>("");
  const [searchResult, setSearchResult] = useState<CompanySearch[]>([]);
  const [serverError, setServerError] = useState<string>("");
  const [portfolioValue, setPortfolioValue] = useState<string[]>([]);

  const saveResult = (result: CompanySearch[]) =>{
    setSearchResult(result);
  }


  const handleSearchChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSearch(e.target.value);
    console.log(e);
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
     if(portfolioValue.indexOf(value) >= 0){
      return;
     }
     const updated = [...portfolioValue,  value]
     setPortfolioValue(updated);
  }

  const onPortfolioDelete = (e: any) =>{
    e.preventDefault();
    const value =  e.target[0].value;
    var updated = portfolioValue.filter(x => {
        return x !== value;
    });
    setPortfolioValue(updated);
  }
  return (
    <>
      <Search onSearchSubmit={onSearchSubmit} handleSearchChange={handleSearchChange} search={search} ></Search>
      <ListPortfolio PortfolioValues={portfolioValue} onPortfolioDelete={onPortfolioDelete}></ListPortfolio>
      {serverError && <h1>{serverError}</h1>}
      <CardList searchResult={searchResult} onPortfolioCreate={onPortfolioCreate}></CardList>
    </>
  )
}

export default SearchPage