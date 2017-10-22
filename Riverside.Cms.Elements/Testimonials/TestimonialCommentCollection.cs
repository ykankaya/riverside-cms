using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace Riverside.Cms.Elements.Testimonials
{
    public class TestimonialCommentCollection : List<TestimonialComment>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var sdr = new SqlDataRecord(
                new SqlMetaData("TestimonialCommentId", SqlDbType.BigInt),
                new SqlMetaData("SortOrder",            SqlDbType.Int),
                new SqlMetaData("Comment",              SqlDbType.NVarChar, -1),
                new SqlMetaData("Author",               SqlDbType.NVarChar, 100),
                new SqlMetaData("AuthorTitle",          SqlDbType.NVarChar, 100),
                new SqlMetaData("CommentDate",          SqlDbType.NVarChar, 30)
            );

            foreach (TestimonialComment comment in this)
            {
                if (comment.TestimonialCommentId != 0)
                    sdr.SetInt64(0, comment.TestimonialCommentId);
                else
                    sdr.SetDBNull(0);
                sdr.SetInt32(1, comment.SortOrder);
                sdr.SetString(2, comment.Comment);
                sdr.SetString(3, comment.Author);
                sdr.SetString(4, comment.AuthorTitle);
                sdr.SetString(5, comment.CommentDate);
                yield return sdr;
            }
        }
    }
}
