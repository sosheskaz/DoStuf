namespace DoStufLib
{
    /// <summary>
    ///     Outputs message data.
    /// </summary>
    public interface IStatusReporter
    {
        /// <summary>
        ///     Outputs a message.
        /// </summary>
        /// <param name="message"> Message to output. </param>
        void ReportMessage(string message);

        /// <summary>
        ///     Outputs a warning.
        /// </summary>
        /// <param name="warning"> Warning to output. </param>
        void ReportWarning(string warning);

        /// <summary>
        ///     Outputs an error.
        /// </summary>
        /// <param name="error"> Error to output. </param>
        void ReportError(string error);
    }
}