import React from 'react'
import { ClipLoader } from 'react-spinners'
import './Spinner.css'

type Props = {
    isLoading?: boolean
}

const Spinner = ({isLoading = true}: Props) => {
  return (
    <>
        <div id='Loading-spinner' >
            <ClipLoader color='#36d7b7' loading={isLoading} size={35} aria-label='Loading spinner' data-testid='loader' />
        </div>
    </>
  )
}

export default Spinner