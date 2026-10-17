#! /bin/perl

#AHC069のエラーチェック用スクリプト

$project = $ARGV[0];
-d $project || die "$project not exists.\n";
my @s, @t, @p, @v;
opendir(DIR, "in");
while ($file = readdir(DIR)) {
	next if $file !~ /\.txt$/;
	print "$file\n";
	$g = 0;
	open(IN, "in/$file");
	$line = <IN>;
	chomp($line);
	($N, $M, $R) = split($line, /\s+/);
	while (<IN>) {
		#print;
		chomp;
		($i, $s, $t, $p, $v) = split(/\s+/);
		#push(@s, $s);
		#push(@t, $t);
		#push(@p, $p);
		#push(@v, $v);
		$s[$i] = $s;
		$t[$i] = $t;
		$p[$i] = $p;
		$v[$i] = $v;
		#print "$i, $s, $t, $p, $v\n";
		#print "group $i $s[$i] $t[$i] $p[$i] $v[$i]\n";
	}
	close(IN);
	$i = 998;
	#print "group $i $s[$i] $t[$i] $p[$i] $v[$i]\n";
	#print "cat in/$file | $project/bin/Debug/$project.exe |\n";
	open(IN, "cat in/$file | $project\\bin\\Debug\\$project.exe |");
	for ($m = 0; $m < 1000; $m++) {
		#print "group $m $s[$m] $t[$m] $p[$m] $v[$m]\n";
		$line = <IN>;
		#print $line;
		chomp($line);
		$line =~ /^[0-9]+$/ || die "$file: not number: $line\n";
		$replace = $line;
		if ($line > 0) {#replace
			for ($i = 0; $i < $replace; $i++) {
				$no = <IN>;
				#print $no;
				chomp($no);
				$no =~ /^[0-9]+$/ || die "$file: not number: $no\n";
				for ($g = 0; $g < $p[$no]; $g++) {
					$line = <IN>;
					#print $line;
					chomp($line);
					$line =~ /^[0-9]+\s+[0-9]+$/ || die "$file: no pos: $line\n";
				}
				#print "replace $no ($i) done\n";
			}
		}
		$line = <IN>;
		#print $line;
		chomp($line);
		$line =~ /^(Yes|No)$/ || die "not Yes/No: $line; $m, $replace\n";
		#print "p[$m] = $p[$m]\n";
		if ($line eq "Yes") {
			for ($g = 0; $g < $p[$m]; $g++) {
				$line = <IN>;
				#print $line;
				chomp($line);
				$line =~ /^[0-9]+\s+[0-9]+$/ || die "$file: no pos: $line\n";
			}
		}
	}
	close(IN);
}
closedir(DIR);
