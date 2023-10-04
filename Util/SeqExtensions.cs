namespace Ace.Util;

public static class SeqExtensions
{
   public static Seq<T> ForceEvaluation<T>(this Seq<T> seq) => seq.ToArray().ToSeq();
}