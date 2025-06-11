import React, { SyntheticEvent } from 'react'
import { CompanySearch } from '../../company'
import AddPortfolio from '../Portfolio/AddPortfolio/AddPortfolio'
import { Link } from 'react-router'

interface Props {
    Id: string,
    company: CompanySearch,
    onPortfolioCreate : (e: SyntheticEvent) => void,
}

const Card : React.FC<Props> = ({Id, company, onPortfolioCreate}: Props) : JSX.Element => {
  return (
    <div className="flex flex-col items-center justify-between w-full p-6 bg-slate-100 rounded-lg md:flex-row"
      key={Id}
      id={Id}
    >
      <Link to={`/company/${company.symbol}/company-profile` } className="font-bold text-center text-veryDarkViolet md:text-left">
        {company.name} ({company.symbol})
      </Link>
      <p className="text-veryDarkBlue">{company.currency}</p>
      <p className="font-bold text-veryDarkBlue">
        {company.exchangeShortName} - {company.stockExchange}
      </p>
        <AddPortfolio onPortfolioCreate={onPortfolioCreate} value={company.symbol}></AddPortfolio>
    </div>
  )
}

export default Card