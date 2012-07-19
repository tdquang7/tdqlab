/*
 * Copyright 1997, Regents of the University of Minnesota
 *
 * main.c
 *
 * This is the entry point of the ILUT
 *
 * Started 10/19/95
 * George
 *
 * $Id: parmetis.c,v 1.5 2003/07/30 21:18:54 karypis Exp $
 *
 */

#include <parmetisbin.h>

/*************************************************************************
* Let the game begin
**************************************************************************/
int main(int argc, char *argv[])
{
	char* infoPath = "F:\\Lab\\Triangles data\\info.txt";
	char* xadjPath = "F:\\Lab\\Triangles data\\xadj.txt";
	char* adjncyPath = "F:\\Lab\\Triangles data\\ajdncy.txt";
	char* outputPath = "F:\\Lab\\Triangles data\\result.txt";

	FILE* infoFile = fopen(infoPath, "r");
	FILE* xadjFile = fopen(xadjPath, "r");
	FILE* adjncyFile = fopen(adjncyPath, "r");
	FILE* outputFile = fopen(outputPath, "w");

	int nodesCount;
	int edgesCount;
	
	idx_t* xadj;	// Restrict begins and ends of adjacent vertex
	idx_t* adjncy;	// List of adjacent vertices
	idx_t* vwgt = NULL;		// Weight on vertex
	idx_t* adjwgt = NULL;	// Weight on adjacent vertex list
	idx_t wgtflag = 0;		// No weight
	idx_t numflag = 0;		// Numbering scheme - start from 0
	idx_t ncon = 1;			// Number of weights each vertex has
	idx_t nparts = 2;		// Number of partitions
	real_t* tpwgts = NULL;  // Fraction of vertex weight that should be distributed each domain
	real_t* ubvec = NULL;   // Imbalance tolerance for each vertex weight
	idx_t* options = NULL;  // Additional parameters
	idx_t edgecut;
	idx_t* part = NULL;	    // Result of partitioning
	idx_t* vtxdist = NULL;  // Vertex distribution among processor
	const int CPUNUM = 1;   // Number of CPU
	int vertexPerCpu;
	MPI_Comm comm;

	idx_t npes, mype;


	int i;
	int temp;

	// Get the number of nodes and edges
	fscanf(infoFile, "%d", &nodesCount);
	fscanf(infoFile, "%d", &edgesCount);
	fclose(infoFile);

	// Get the xadj list
	xadj = (idx_t*) malloc ((nodesCount + 1) * sizeof(idx_t));
	for (i = 0; !feof(xadjFile); i++)
	{
		fscanf(xadjFile, "%d", &temp);
		xadj[i] = temp;
	}
	fclose(xadjFile);

	// Get the adjncy list
	adjncy = (idx_t*) malloc(edgesCount * sizeof(idx_t));
	for(i = 0; !feof(adjncyFile); i++)
	{
		fscanf(adjncyFile, "%d", &temp);
		adjncy[i] = temp;
	}
	fclose(adjncyFile);

	// Fraction of vertex weight that should be distributed to each sub-domain 
	// for each balance constraint
	tpwgts = (real_t*) malloc(ncon * nparts * sizeof(real_t));

	for(i = 0; i < ncon * nparts; i++)
	{
		tpwgts[i] = 1.0 / nparts;
	}

	ubvec = (real_t*) malloc(ncon * sizeof(real_t));
	for(i = 0; i < ncon; i++)
	{
		ubvec[i] = 1.05;
	}

	// Additional parameters - all default here
	options = (idx_t*) malloc(3 * sizeof(idx_t));
	for(i = 0; i < 3; i++)
	{
		options[i] = 0;
	}

	part = (idx_t*) malloc(nodesCount * sizeof(idx_t));

	// Prepare vertex distribution on each cpu
	vertexPerCpu = nodesCount / CPUNUM;
	vtxdist = (idx_t*) malloc((CPUNUM + 1) * sizeof(idx_t));
	
	for(i = 0; i < CPUNUM + 1; i++)
	{
		vtxdist[i] = i * vertexPerCpu;
	}
		
	// Not sure if needed, copy from example
	MPI_Init(&argc, &argv);
	MPI_Comm_dup(MPI_COMM_WORLD, &comm);
	gkMPI_Comm_size(comm, &npes); // Is npes the number of CPU???
	gkMPI_Comm_rank(comm, &mype);
	
	// Call function to partition
	ParMETIS_V3_PartKway(vtxdist, xadj, adjncy, vwgt, adjwgt, &wgtflag, &numflag, &ncon, &nparts, tpwgts, ubvec, 
          options, &edgecut, part, &comm);
	
	//  Put output result to file
	for(i=0;i<nodesCount; i++)
	{
		fprintf(outputFile, "%d %d\n", i, part[i]);
	}

	fclose(outputFile);
	
	MPI_Comm_free(&comm);
	MPI_Finalize();

  //idx_t i, j, npes, mype, optype, nparts, adptf, options[10];
  //idx_t *part=NULL, *sizes=NULL;
  //graph_t graph;
  //real_t ipc2redist, *xyz=NULL, *tpwgts=NULL, ubvec[MAXNCON];
  
  //if (mype == 0) 
  //  printf("reading file: %s\n", argv[1]);
  //ParallelReadGraph(&graph, argv[1], comm);

  //rset(graph.ncon, 1.05, ubvec);
  //tpwgts = rmalloc(nparts*graph.ncon, "tpwgts");
  //rset(nparts*graph.ncon, 1.0/(real_t)nparts, tpwgts);

  //nvtxs = graph.vtxdist[mype+1]-graph.vtxdist[mype];
  //nedges = graph.xadj[nvtxs];
  //printf("%"PRIDX" %"PRIDX"\n", isum(nvtxs, graph.xadj, 1), isum(nedges, graph.adjncy, 1));
  //*/

  //part  = ismalloc(graph.nvtxs, mype%nparts, "main: part");
  //sizes = imalloc(2*npes, "main: sizes");

  //    ParMETIS_V3_PartKway(graph.vtxdist, graph.xadj, graph.adjncy, graph.vwgt, 
  //        graph.adjwgt, &wgtflag, &numflag, &graph.ncon, &nparts, tpwgts, ubvec, 
  //        options, &edgecut, part, &comm);
  //    WritePVector(argv[1], graph.vtxdist, part, MPI_COMM_WORLD); 

  return 0;
}


/*************************************************************************
* This function changes the numbering to be from 1 instead of 0
**************************************************************************/
void ChangeToFortranNumbering(idx_t *vtxdist, idx_t *xadj, idx_t *adjncy, idx_t mype, idx_t npes)
{
  idx_t i, nvtxs, nedges;

  nvtxs = vtxdist[mype+1]-vtxdist[mype];
  nedges = xadj[nvtxs];

  for (i=0; i<npes+1; i++)
    vtxdist[i]++;
  for (i=0; i<nvtxs+1; i++)
    xadj[i]++;
  for (i=0; i<nedges; i++)
    adjncy[i]++;

  return;
}
