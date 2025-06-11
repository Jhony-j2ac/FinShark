import React from 'react'
import { JsxElement } from 'typescript'
import DeletePortfolio from '../DeletePortfolio/DeletePortfolio'
import { Link } from 'react-router'

interface Props {
    porfolioName : string,
    onPortfolioDelete: (e: any) => void,
}

const CardPortfolio : React.FC<Props> = ({porfolioName, onPortfolioDelete}: Props) : JSX.Element => {
  return (
    <div className="flex flex-col w-full p-8 space-y-4 text-center rounded-lg shadow-lg md:w-1/3">
      <Link to={`/company/${porfolioName}/company-profile`} className="pt-6 text-xl font-bold">
        {porfolioName}
      </Link>
      <DeletePortfolio
        portfolioValue={porfolioName}
        onPortfolioDelete={onPortfolioDelete}
      />
    </div>
  )
}

export default CardPortfolio