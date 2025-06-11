import React, { SyntheticEvent } from 'react'
import Card from '../Card/Card'
import { CompanySearch } from '../../company'
import { v4 as uuid } from 'uuid'

interface Props {
  searchResult: CompanySearch[],
  onPortfolioCreate: (e: SyntheticEvent) => void
}

const CardList : React.FC<Props> = (props: Props) : JSX.Element => {
  return (
    <div>
        {
          props.searchResult.length > 0 ? (
            props.searchResult.map(company => (
              <Card Id={company.symbol} key={uuid()} company={company} onPortfolioCreate={props.onPortfolioCreate} ></Card>
            ))
          ) : 
          (
            <p className='mb-2 mt-3 text-xl font-semibold text-center md:text-xl'>No results</p>
          )
        }
    </div>
  )
}

export default CardList