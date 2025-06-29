import { CommentGet } from '../../Models/Comment';
import Spinner from '../Spinner/Spinner';
import StockCommentListItem from './StockCommentListItem';

type Props = {
    comments: CommentGet[];
}

const StockCommentList = ({comments}: Props) => {
  
  return (
    comments?.length > 0 ? (
      <>
        {comments.map((comment) => (<StockCommentListItem comment={comment} />))}
      </>
    ) : (<Spinner />)
  )
}

export default StockCommentList